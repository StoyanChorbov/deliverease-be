import type { LocationDto } from './types/LocationDto';
import type {
	LocationQueryResponseDto,
	LocationQueryFeatureDto,
} from './types/LocationQueryResponseDto';

export const getLocationSuggestions = async (
	input: string
): Promise<LocationDto[]> => {
	const runtimeConfig = useRuntimeConfig();
	const accessToken = runtimeConfig.public.mapboxKey;

	const res = await $fetch<LocationQueryResponseDto>(
		`https://api.mapbox.com/search/geocode/v6/forward?q=${input}&country=bg&types=address%2Csecondary_address%2Cstreet%2Cneighborhood%2Cplace%2Cdistrict%2Clocality%2Cpostcode&access_token=${accessToken}`
	);

    // console.log(res);

	return res.features.map((feature: LocationQueryFeatureDto) => {
		const context = feature.properties.context;
		return {
			place: context.place ? context.place.name : '',
			street: context.street ? context.street.name : '',
			number: context.address
				? parseFloat(context.address.address_number)
				: 0,
			region: context.region ? context.region.name : '',
			longitude: feature.geometry.coordinates[0],
			latitude: feature.geometry.coordinates[1],
		};
	});
};

export const formatLocations = (locations: LocationDto[]) => {
	return locations.map((location) => {
		
	});
}

export const formatLocation = (location: LocationDto) => {
	let displayText = '';

	if (location.street != '') {
		if (location.number != 0) {
			displayText += `${location.street} ${location.number}, `;
		} else {
			displayText += `${location.street}, `;
		}
	}

	if (location.place != '') {
		displayText += `${location.place}, `;
	}

	if (location.region != '') {
		displayText += `${location.region}`;
	}

	return {
		...location,
		displayText: displayText,
	};
}
