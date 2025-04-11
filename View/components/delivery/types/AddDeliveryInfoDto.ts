import type {DeliveryCategory} from "~/components/delivery/types/DeliveryCategory";

export interface AddDeliveryInfoDto {
    packageName: string;
    startLocation: string;
    endLocation: string;
    description: string;
    category: DeliveryCategory;
    isFragile: boolean;
}