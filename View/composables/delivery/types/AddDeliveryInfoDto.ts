import type {DeliveryCategory} from "~/composables/delivery/types/DeliveryCategory";

export interface AddDeliveryInfoDto {
    packageName: string;
    startLocation: string;
    endLocation: string;
    description: string;
    category: DeliveryCategory;
    isFragile: boolean;
}