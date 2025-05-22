import type {LocationDto} from "~/composables/delivery/types/LocationDto";

export interface DeliveryDetailsDto {
    id: number;
    name: string;
    category: string;
    description: string;
    startLocation: LocationDto;
    endLocation: LocationDto;
    recipients: string[];
    deliverer?: string;
}