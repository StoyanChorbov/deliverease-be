import type {LocationDto} from "~/composables/delivery/types/LocationDto";

export interface DeliveryRowDto {
    id?: string;
    name?: string;
    category?: string;
    startingLocation?: LocationDto;
    endingLocation?: LocationDto;
    isFragile?: boolean;
}