const port: string = ':5000';
const protocol: string = window.location.protocol;
const hostname: string = window.location.hostname;
const ip: string = `${hostname}${port}`;
const apiUrl: string = `${protocol}//${ip}`;

export const environment = {
  production: false,
  baseUrl: `${apiUrl}/`,
  apiUrl: `${apiUrl}/api/`,
  userCounterUrl: `${apiUrl}/UserCounter`,
  loginDetectUrl: `${apiUrl}/LoginDetect`,
  hostApplicationLifetimeUrl: `${apiUrl}/HostApplicationLifetime`,
  allowedDomains: [ip],
  disallowedRoutes: [`${apiUrl}/api/auth`],
};
