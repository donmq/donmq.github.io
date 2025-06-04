import { Injectable } from "@angular/core";

@Injectable({
    providedIn: "root",
})
export class ZIndexUtils {
    static generateZIndex = (key: any, baseZIndex: any) => {
        let zIndexes: any = [];
        let lastZIndex = zIndexes.length > 0 ? zIndexes[zIndexes.length - 1] : { key, value: baseZIndex };
        let newZIndex = lastZIndex.value + (lastZIndex.key === key ? 0 : baseZIndex) + 2;

        zIndexes.push({ key, value: newZIndex });

        return newZIndex;
    };

    static revertZIndex = (zIndex) => {
        let zIndexes: any = [];
        zIndexes = zIndexes.filter((obj) => obj.value !== zIndex);
    };

    static getCurrentZIndex = () => {
        let zIndexes: any = [];
        return zIndexes.length > 0 ? zIndexes[zIndexes.length - 1].value : 0;
    };

    static getZIndex = (el: any) => {
        return el ? parseInt(el.style.zIndex, 10) || 0 : 0;
    };

    static get = (el: any) => ZIndexUtils.getZIndex(el);

    static set(key: any, el: any, baseZIndex: any) {
        if (el) {
            el.style.zIndex = String(ZIndexUtils.generateZIndex(key, baseZIndex));
        }
    }

    static clear(el: any) {
        if (el) {
            ZIndexUtils.revertZIndex(ZIndexUtils.getZIndex(el));
            el.style.zIndex = '';
        }
    }
}