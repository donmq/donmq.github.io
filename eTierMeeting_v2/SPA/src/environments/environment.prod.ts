const protocol: string = 'https:';
const hostname: string = window.location.hostname;
const port: string = ':8181'; //Use 8282 port when test server have the same ip address with the official server
const domain: string = `${hostname}${port}`;
const baseUrl: string = `${protocol}//${domain}`;

export const environment = {
  production: true,
  allowedDomains: [domain],
  disallowedRoutes: [`${domain}/api/auth`],
  baseUrl: `${baseUrl}/`,
  apiUrl: `${baseUrl}/api/`,
  signalrUrl: `${baseUrl}/signalrHub`,
};
