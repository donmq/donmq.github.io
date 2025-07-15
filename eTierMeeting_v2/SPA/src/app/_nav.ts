import { Injectable } from '@angular/core';
import { INavData } from '@coreui/angular';
@Injectable({
  providedIn: 'root'
})
export class NavItem {
  navItems: INavData[] = [];
  constructor() {
  }
  getNav() {
    this.navItems = [];
    const navItemMaintain = {
      name: 'Maintain',
      icon: 'fa fa-cogs',
      url: 'maintain',
      children: [
        {
          name: 'Trang Quản Trị',
          url: '/admin',
          class: 'menu-margin'
        }
      ]
    };


    const navItemTransaction = {
      name: 'Transaction',
      icon: 'fa fa-balance-scale',
      url: 'transaction',
      children: [
        {
          name: 'Di Chuyển Máy',
          url: '/move',
          class: 'menu-margin'
        },
        {
          name: 'Kiểm Kê',
          url: '/inventoryv2',
          class: 'menu-margin'
        },
        {
          name: 'Kiểm Tra Máy',
          url: '/checkmachine',
          class: 'menu-margin'
        },
        {
          name: 'Kiểm tra an toàn máy móc',
          url: '/checkmachinesafety',
          class: 'menu-margin'
        },
        {
          name: 'Assets Lend Maintain',
          url: '/assetslendmaintain',
          class: 'menu-margin'
        },
      ]
    };

    const navItemKanban = {
      name: 'Kanban',
      icon: 'fa fa-desktop',
      url: 'kanban',
      children: []
    };

    const navItemReport = {
      name: 'Report',
      icon: 'fa fa-newspaper-o',
      url: 'report',
      children: [
        {
          name: 'Lịch Sử Kiểm Tra Máy',
          url: '/historycheckmachine',
          class: 'menu-margin'
        },
        {
          name: 'Lịch Sử Kiểm Kê',
          url: '/historyinventory',
          class: 'menu-margin'
        },
        {
          name: 'Báo Cáo Kiểm Kê',
          url: '/reportinventory',
          class: 'menu-margin'
        },
        {
          name: 'Lịch Sử Chuyển Máy',
          url: '/historymove',
          class: 'menu-margin'
        },
        {
          name: 'Thống Kê Máy',
          url: '/reportmachine',
          class: 'menu-margin'
        },
        {
          name: 'Báo cáo kiểm tra an toàn máy móc',
          url: '/reportcheckmachinesafety',
          class: 'menu-margin'
        },
      ]
    };
    const navItemQuery = {
      name: 'Query',
      icon: 'fa fa-search',
      url: 'query',
      children: [
        {
          name: 'Danh Mục Máy',
          url: '/home',
          class: 'menu-margin'
        },
      ]
    };


    this.navItems.push(navItemMaintain);
    this.navItems.push(navItemTransaction);
    this.navItems.push(navItemKanban);
    this.navItems.push(navItemReport);
    this.navItems.push(navItemQuery);
    return this.navItems;

  }
}
