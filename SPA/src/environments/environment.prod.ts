const port = 9005;
const ip = `${window.location.hostname}:${port}`;
const apiUrl = `https://${ip}`;


export const environment = {
  production: true,
  apiUrl: `${apiUrl}/api/`,
  baseUrl: `${apiUrl}/`,
  whitelistedDomains: [ip],
  blacklistedRoutes: [`${ip}/api/auth`],
  imageUserUrl: `${apiUrl}/uploaded/images/users/`,
  imageUserDefault: 'assets/img/default-150x150.png',
};
