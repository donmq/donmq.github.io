export interface ISidebarItem {
  id: string;
  svgPath: string;
  label: string;
  outlined?: boolean;
}

export type SidebarItems = ISidebarItem[];
