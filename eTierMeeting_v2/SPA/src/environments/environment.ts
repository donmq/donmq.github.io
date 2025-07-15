const protocol: string = 'https:';
const hostname: string = window.location.hostname;
const port: string = ':5001';
const domain: string = `${hostname}${port}`;
const baseUrl: string = `${protocol}//${domain}`;

export const environment = {
  production: false,
  allowedDomains: [domain],
  disallowedRoutes: [`${domain}/api/auth`],
  baseUrl: `${baseUrl}/`,
  apiUrl: `${baseUrl}/api/`,
  signalrUrl: `${baseUrl}/signalrHub`,
};
