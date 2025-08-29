import { Injectable } from '@angular/core';
import { NavConstants } from '@constants/nav.constants';
import { INavData } from '@coreui/angular';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { AuthProgram, CodeInformation, DirectoryInfomation, ProgramInfomation } from '@models/auth/auth';
import { LangConstants } from '@constants/lang-constants';
import { CommonService } from '@services/common.service';
@Injectable({ providedIn: 'root' })
export class Nav {
  filter_menus: string[] = ['4.1.2', '4.1.3', '4.1.4', '4.1.5'];
  constructor(private service: CommonService) { }
  getNav() {
    const authProgram = this.service.authPrograms
    return authProgram && authProgram.directories ? this.nav(authProgram) : []
  }
  private nav = (authProgram: AuthProgram): INavData[] => {
    const lang: string = localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.EN;
    const code_lang: CodeInformation[] = JSON.parse(localStorage.getItem(LocalStorageConstants.CODE_LANG)) || [];
    const user_directory: DirectoryInfomation[] = authProgram.directories || [];
    const user_roles: ProgramInfomation[] = authProgram.programs || [];
    return user_directory.map((dir, dirIndex) =>
      <INavData>{
        name: `${dir.seq}. ${code_lang.find(val => val.code == dir.directory_Code && val.kind == 'D')?.translations
            ?.find(val => val.lang == lang)?.name ?? dir.directory_Name
          }`,
        icon: NavConstants[dirIndex]?.icon ?? 'fa fa-window-maximize',
        url: dir.directory_Name?.toUrl(),
        children: [...user_roles
          .filter(pro => pro.parent_Directory_Code == dir.directory_Code && !this.filter_menus.includes(pro.program_Code))
          .map(pro =>
            <INavData>{
              name: `${pro.program_Code} ${code_lang.find(val => val.code == pro.program_Code && val.kind == 'P')?.translations
                  ?.find(val => val.lang == lang)?.name ?? pro.program_Name
                }`,
              url: dir.directory_Name?.toUrl() + '/' + pro.program_Name?.toUrl(),
              class: 'menu-margin'
            }
          )
        ]
      }
    ).filter(dir => dir.children.length > 0);
  }
}

