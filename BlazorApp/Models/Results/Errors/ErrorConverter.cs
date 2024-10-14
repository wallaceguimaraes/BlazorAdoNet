using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Models.Results.Errors
{
    public class ErrorConverter
    {
        public ErrorConverter()
        {
            
        }
        public string MapError(string error)
        {
            switch (error){
                case "USER_NOT_FOUND":
                    return "Usuário não existe!";
                case "PROJECT_NOT_FOUND":
                    return "Projeto não existe!";
                case "WRONG_PASSWORD":
                    return "Senha incorreta!";
                case "CREATE_USER_ERROR":
                    return "Ocoreu erro na criação do usuário!";
                case "USER_ALREADY_CREATED":
                    return "Usuário já foi criado anteriormente!";
                case "AUTHENTICATION_ERROR":
                    return "Ocorreu algum erro ao tentar se autenticar no sistema!";
                case "CREATE_PROJECT_ERROR":
                    return "Ocorreu um erro ao tentar criar o projeto!";
                case "INVALID_DATE":
                    return "Data inválida!";
                case "GET_PROJECT_ERROR":
                    return "Ocorreu erro ao tentar obter dados do projeto!";
                case "UPDATE_PROJECT_ERROR":
                    return "Ocorreu um erro ao tentar atualizar o registro!";
            }

            return "Ocorreu algum erro!";
        }
    }
}