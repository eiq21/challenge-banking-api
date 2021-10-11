import { env } from './.env';
export const environment = {
  production: false,
  version: env.npm_package_version + '-dev',
  serverIdentityUrl: 'http://localhost:7000/api/',
  serverExchangeUrl: 'http://localhost:5000/api/',
  defaultLanguage: 'es-PE',
  supportLanguages: ['es-US', 'es-PE'],
};
