<script setup lang="ts">
import { formatLocation } from '~/composables/delivery/LocationService';
import { type DeliveryRowDto } from '~/composables/delivery/types/DeliveryRowDto';

interface DeliveriesSectionProps {
	sectionTitle: string;
	deliveries: DeliveryRowDto[];
}

const props = defineProps<DeliveriesSectionProps>();
</script>

<template>
	<v-container>
		<p class="text-h2 font-weight-bold text-center">{{ sectionTitle }}</p>
		<DeliveryRow
			v-for="delivery in deliveries"
			:id="delivery.id"
			:deliveryName="delivery.name"
			:startLocation="formatLocation(delivery.startingLocationDto).displayText"
			:endLocation="formatLocation(delivery.endingLocationDto).displayText"
		/>
	</v-container>
</template>

<style scoped></style>
