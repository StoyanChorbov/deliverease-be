interface LocationQueryResponseDto {
    type: string;
    features: LocationQueryFeatureDto[];
    attribution: string;
}

interface LocationQueryFeatureDto {
    type: string;
    id: string;
    geometry: LocationQueryGeometryDto;
    properties: LocationQueryPropertiesDto;
}

interface LocationQueryGeometryDto {
    type: string;
    coordinates: number[];
}

interface LocationQueryPropertiesDto {
    mapbox_id: string | undefined;
    feature_type: string | undefined;
    name: string | undefined;
    name_preferred: string | undefined;
    place_formatted: string | undefined;
    full_address: string | undefined;
    context: LocationQueryContextDto;
}

interface LocationQueryContextDto {
    adddress: LocationQueryAddressDto | undefined;
    street: LocationQueryStreetDto | undefined;
    postcode: LocationQueryPostcodeDto | undefined;
    place: LocationQueryPlaceDto | undefined;
    region: LocationQueryRegionDto | undefined;
    country: LocationQueryCountryDto | undefined;
}

interface LocationQueryAddressDto {
    mapbox_id: string;
    address_number: string;
    street_name: string;
    name: string;
}

interface LocationQueryStreetDto {
    mapbox_id: string;
    name: string;
}

interface LocationQueryPostcodeDto {
    mapbox_id: string;
    name: string;
}

interface LocationQueryPlaceDto {
    mapbox_id: string;
    name: string;
}

interface LocationQueryRegionDto {
    mapbox_id: string;
    name: string;
    region_code: string | undefined;
    region_code_full: string | undefined;
}

interface LocationQueryCountryDto {
    mapbox_id: string;
    name: string;
    country_code: string | undefined;
    country_code_alpha_3: string | undefined;
}

export type { LocationQueryResponseDto, LocationQueryFeatureDto }