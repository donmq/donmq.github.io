import { SearchMachineInventory } from '../_models/search-machine-inventory';

export interface InventoryParams {
    idInventory: number;
    idPlno: string;
    fromDateTime: string;
    toDateTime: string;
    listMachineInventory: SearchMachineInventory
    erorr: number;
    inventoryID: number;
    typeFile: string;
}