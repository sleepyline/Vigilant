namespace VigiLant.Services
{
    public interface IHashService
    {

        string GerarHash(string senha);
        bool VerificarHash(string senhaDigitada, string senhaHashArmazenada);

    }
}