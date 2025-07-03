import { LocalStorageConstants } from '@constants/storage.constants';
import { ServerInfo } from '../_models/server-info';
import { CommonService } from './common.service';

export function appInitializer(commonService: CommonService) {
  return () => new Promise(resolve => {
    commonService.getServerInfo()
      .subscribe({
        next: (res: ServerInfo) => {
          localStorage.setItem(LocalStorageConstants.FACTORY, res.factory);
          localStorage.setItem(LocalStorageConstants.AREA, res.area);
        }
      })
      .add(resolve);
  });
}
