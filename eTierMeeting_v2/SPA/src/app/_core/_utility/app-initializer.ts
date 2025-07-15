import { ServerInfo } from "../_models/server-info";
import { AuthService } from "../_services/auth.service";

export function appInitializer(authService: AuthService) {
  return () => new Promise(resolve => {
    authService.getServerInfo()
      .subscribe({
        next: (res: ServerInfo) => {
          localStorage.setItem('factory', res.factory);
          localStorage.setItem('area', res.area);
          localStorage.setItem('local', res.local);
        }
      })
      .add(resolve);
  });
}