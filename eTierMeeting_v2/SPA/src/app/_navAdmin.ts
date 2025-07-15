import { INavData } from '@coreui/angular';

export const navAdmin: INavData[] = [
  {
    name: 'Home',
    url: '/home',
    icon: 'icon-home'
  },
  {
    name: 'PDC Management',
    url: '/admin/pdc',
    icon: 'fa fa-product-hunt'
  },
  {
    name: 'Building Management',
    url: '/admin/building',
    icon: 'fa fa-building-o'
  },
  {
    name: 'Cell Management',
    url: '/admin/cell',
    icon: 'fa fa-building'
  },
  {
    name: 'Cell_Plno Management',
    url: '/admin/cellplno',
    icon: 'fa fa-tachometer'
  },
  {
    name: 'Employee Management',
    url: '/admin/employee',
    icon: 'fa fa-users'
  },
  {
    name: 'User Management',
    url: '/admin/user',
    icon: 'fa fa-user-circle'
  },
  {
    name: 'Inventory Management',
    url: '/admin/inventory',
    icon: 'fa fa-calendar'
  },
  {
    name: 'Preliminary Management',
    url: '/admin/preliminary',
    icon: 'fa fa-user-circle'
  },
  {
    name: 'Logout',
    url: '/admin/logout',
    icon: 'fa fa-power-off'
  }
];
