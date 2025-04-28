import type {DeliveryCategory} from "~/composables/delivery/types/DeliveryCategory";
import type {LocationDto} from "~/composables/delivery/types/LocationDto";

export interface AddDeliveryInfoDto {
    name: string;
    startLocation: LocationDto;
    startLocationRegion: string,
    endLocation: LocationDto;
    endLocationRegion: string,
    description: string;
    category: DeliveryCategory;
    isFragile: boolean;
}