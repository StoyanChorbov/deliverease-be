import type {LocationDto} from "~/composables/delivery/types/LocationDto";

export interface DeliveryRowDto {
    id: string;
    name: string;
    category: string;
    startingLocationDto: LocationDto;
    endingLocationDto: LocationDto;
    isFragile: boolean;
}