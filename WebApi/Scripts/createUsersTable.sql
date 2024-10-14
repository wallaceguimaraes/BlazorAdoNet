USE `BancoDesenvolvimento`;

CREATE TABLE IF NOT EXISTS `Usuarios` (
    `Id` BIGINT NOT NULL AUTO_INCREMENT,      
    `Nome` VARCHAR(255) NOT NULL,                
    `Email` VARCHAR(255) NOT NULL UNIQUE,        
    `Senha` VARCHAR(255) NOT NULL,            
    `VersaoToken` BIGINT NOT NULL DEFAULT 0,    
    `Salt` VARCHAR(255) NOT NULL,
    `DataCriacao` DATETIME,                
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
