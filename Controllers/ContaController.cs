// Controllers/ContaController.cs
using Microsoft.AspNetCore.Mvc;
using VigiLant.Services;
using VigiLant.Models;
using VigiLant.Models.Enum;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

public class ContaController : Controller
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHashService _hashService;

    public ContaController(IUsuarioRepository usuarioRepository, IHashService hashService)
    {
        _usuarioRepository = usuarioRepository;
        _hashService = hashService;
    }

    private IActionResult RedirecionarParaHome() => RedirectToAction("Index", "Home");

    // --- Login (GET) ---
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated) return RedirecionarParaHome();
        ViewData["Title"] = "Login";
        return View();
    }

    // --- Login (POST) ---
    [HttpPost]
    public async Task<IActionResult> Login(string email, string senha)
    {
        ViewData["Title"] = "Login";
        
        // 1. Buscar usuário
        var usuario = await _usuarioRepository.BuscarPorEmail(email);

        // 2. Validar credenciais
        if (usuario == null || !_hashService.VerificarHash(senha, usuario.SenhaHash))
        {
            ViewBag.Erro = "E-mail ou Senha inválidos.";
            return View();
        }

        // 3. Criar Claims (Identidade)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.cargo.ToString()) // Adiciona a Role/Cargo
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            // isPersistent: true para "Lembrar-me"
            IsPersistent = false, 
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        // 4. Logar o usuário (Cria o Cookie)
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirecionarParaHome();
    }

    // --- Register (GET) ---
    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated) return RedirecionarParaHome();
        ViewData["Title"] = "Register";
        return View();
    }

    // --- Register (POST) ---
    [HttpPost]
    public async Task<IActionResult> Register(string nome, string email, string senha)
    {
        ViewData["Title"] = "Register";
        // 1. Verificar se o usuário já existe
        if (await _usuarioRepository.BuscarPorEmail(email) != null)
        {
            ViewBag.Erro = "Este e-mail já está cadastrado.";
            return View();
        }

        // 2. Criar novo usuário e hash da senha
        var novoUsuario = new Usuario
        {
            Nome = nome,
            Email = email,
            SenhaHash = _hashService.GerarHash(senha), // Usa o Hash PBKDF2
            cargo = Cargo.Colaborador // Define um cargo padrão para novos registros
        };

        // 3. Salvar no DB
        await _usuarioRepository.Adicionar(novoUsuario);

        // Por simplicidade, redireciona para o login após o registro bem-sucedido
        return RedirectToAction("Login");
    }

    // --- Logout (POST) ---
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        // Remove o cookie de autenticação
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        // CORREÇÃO: Volta para a tela de Login
        return RedirectToAction("Login"); 
    }
}