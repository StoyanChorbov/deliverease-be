<script setup lang="ts">
import type { DeliveryDetailsDto } from '~/composables/delivery/types/DeliveryDetailsDto';
import { useApi, useAuth } from '#build/imports';

const route = useRoute();
const id = route.params.id;

const router = useRouter();
const auth = useAuth();

const delivery = ref<DeliveryDetailsDto>();

const handleDeliverPackage = async () => {
	await useApi(`/deliveries/deliver`, {
		method: 'POST',
		body: { deliveryId: id },
	})
		.then(() => {
			router.push('/');
		})
		.catch((error) => {
			throw new Error('Error creating delivery: ', error);
		});
};

const showDeliveryBtn = computed(() => {
	const deliveryInfo = delivery.value;
	const username = auth.getUsername();
	return (
		deliveryInfo?.deliverer !== undefined &&
		deliveryInfo?.sender !== username &&
		deliveryInfo?.deliverer !== username
	);
});

const handleLoad = async (id: string) => {
	const res = await useApi<DeliveryDetailsDto>(`/deliveries/${id}`)
		.then((res) => {
			delivery.value = res;
		})
		.catch((error) => {
			throw new Error('Error creating delivery: ', error);
		});
};

onMounted(async () => {
	await handleLoad(typeof id === 'string' ? id : id[0]);
});
</script>

<template>
	<v-container>
		<LazyMapWithMarkers />
		<v-col>
			<v-card>
				<v-card-title>Delivery Details</v-card-title>
				<v-card-text v-if="delivery">
					<p>Name: {{ delivery.name }}</p>
					<p>Recipients: {{ delivery.recipients?.join(', ') }}</p>
					<p>Category: {{ delivery.category }}</p>
					<p>Description: {{ delivery.description }}</p>
				</v-card-text>
				<v-row v-else>Delivery not found</v-row>
			</v-card>
		</v-col>
		<v-btn
			v-if="showDeliveryBtn"
			variant="outlined"
			@click="handleDeliverPackage"
			>Deliver package</v-btn
		>
	</v-container>
</template>
