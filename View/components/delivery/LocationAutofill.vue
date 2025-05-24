<script setup lang="ts">
import type { LocationDto } from '~/composables/delivery/types/LocationDto';
import type { VCombobox } from 'vuetify/components';
import {
	getLocationSuggestions,
	formatLocations,
} from '~/composables/delivery/LocationService';

interface LocationAutofillProps {
	label: string;
	onUpdateLocation: () => void;
}

const props = defineProps<LocationAutofillProps>();
const selectedLocation = defineModel<
	(LocationDto & { displayText: string }) | null
>();

const locationSuggestions = ref<LocationDto[]>([]);
const formattedSuggestions = computed(
	() =>
		(locationSuggestions.value = formatLocations(locationSuggestions.value))
);

const getSuggestions = async (input: string) => {
	if (input.length > 2) {
		const res = await getLocationSuggestions(input);
		locationSuggestions.value = res;
	}
};
</script>

<template>
	<v-combobox
		:label="label"
		variant="outlined"
		:items="formattedSuggestions"
		:menu="true"
		:no-filter="true"
		v-model="selectedLocation"
		item-title="displayText"
		return-object
		@update:search="getSuggestions"
		@update:model-value="onUpdateLocation"
	></v-combobox>
</template>
