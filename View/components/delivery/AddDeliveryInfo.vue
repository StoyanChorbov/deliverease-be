<script setup lang="ts">
import { DeliveryCategory } from '~/composables/delivery/types/DeliveryCategory';
import type { AddDeliveryInfoDto } from '~/composables/delivery/types/AddDeliveryInfoDto';
import type { LocationDto } from '~/composables/delivery/types/LocationDto';
import LocationAutofill from '~/components/delivery/LocationAutofill.vue';

interface AddDeliveryInfoProps {
	nextSectionHandler: (addDeliveryInfoDto: AddDeliveryInfoDto) => void;
}

const props = defineProps<AddDeliveryInfoProps>();

const packageName = ref('');
const startLocation = ref<(LocationDto & { displayText: string }) | null>(null);
const endLocation = ref<(LocationDto & { displayText: string }) | null>(null);
const description = ref('');
const category = ref(DeliveryCategory.Other);
const isFragile = ref(false);
const error = ref('');

const handleNext = () => {
	const start = startLocation.value;
	if (start === null) {
		error.value = 'Starting location is required';
		return;
	}
	const end = endLocation.value;
	if (end === null) {
		error.value = 'Ending location is required';
		return;
	}
	const addDeliveryInfoDto: AddDeliveryInfoDto = {
		name: packageName.value,
		startLocation: {
			place: start.place,
			street: start.street,
			number: start.number,
			region: start.region,
			longitude: start.longitude,
			latitude: start.latitude,
		},
		startLocationRegion: start.region,
		endLocation: {
			place: end.place,
			street: end.street,
			number: end.number,
			region: end.region,
			longitude: end.longitude,
			latitude: end.latitude,
		},
		endLocationRegion: end.region,
		description: description.value,
		category: category.value,
		isFragile: isFragile.value,
	};

	// Call the next section handler with the DTO
	props.nextSectionHandler(addDeliveryInfoDto);
};
</script>

<template>
	<v-container class="text-center">
		<h1 class="text-3xl font-bold pb-2">Add delivery</h1>
		<v-form>
			<v-text-field
				variant="outlined"
				v-model="packageName"
				label="Package name"
			/>
			<LocationAutofill
				v-model="startLocation"
				label="Starting Location"
			/>
			<LocationAutofill v-model="endLocation" label="Ending Location" />
			<v-textarea
				variant="outlined"
				v-model="description"
				label="Description"
			/>
			<v-select
				variant="outlined"
				label="Category"
				v-model="category"
				:items="Object.values(DeliveryCategory)"
			/>
			<v-checkbox v-model="isFragile" label="Is this package fragile?" />
			<v-btn
				icon="mdi-arrow-right"
				variant="outlined"
				@click="handleNext"
			></v-btn>
			<v-row v-if="error.length > 0">{{ error }}</v-row>
		</v-form>
	</v-container>
</template>
