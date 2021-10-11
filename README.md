# challenge-banking-api
Desafío técnico net core
![image](https://user-images.githubusercontent.com/18255776/136705403-5cea581f-9c65-464a-9996-fab7e7300281.png)

# Instrucciones de ejecución - Backend

Ejecutar el siguiente comando en la raíz donde se encuentra el archivo docker-compose.yml

```bash
$ docker-compose build
$ docker-compose up
```
### Microservice Identity API(host) 
- http://localhost:7000/swagger

### Microservice Exchange API(host) 
- http://localhost:5000/swagger


# Instrucciones de ejecución - Frontend 
Ejecutar el siguiente comando en la raíz donde se encuentra el proyecto cliente web angular (challenge-banking\src\clients\exchange-web)

```bash
$ npm install
$ ng serve -o
```
-  Consumo de servicio de autenticación JWT

## License

This project is licensed with the [MIT license](LICENSE).
