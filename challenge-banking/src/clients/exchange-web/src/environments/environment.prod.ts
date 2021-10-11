import { env } from './.env';
export const environment = {
  production: true,
  version: env.npm_package_version,
  serverIdentityUrl: 'http://localhost:7000/api/',
  serverExchangeUrl: 'http://localhost:5000/api/',
  defaultLanguage: 'es-PE',
  supportLanguages: ['es-US', 'es-PE'],
};
