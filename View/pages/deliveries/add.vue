<script setup lang="ts">

import AddDeliveryInfo from "~/components/delivery/AddDeliveryInfo.vue";
import {DeliveryCategory} from "~/composables/delivery/types/DeliveryCategory";
import type {AddDeliveryInfoDto} from "~/composables/delivery/types/AddDeliveryInfoDto";
import AddRecipients from "~/components/delivery/AddRecipients.vue";
import {useApi} from "#build/imports";

const addDeliveryInfoDto = ref<AddDeliveryInfoDto>({
    name: "Package",
    startLocation: {
        address: "Address 1",
        city: "City",
        latitude: 24,
        longitude: 24
    },
    startLocationRegion: "Some Region",
    endLocation: {
        address: "Address 2",
        city: "Town",
        latitude: 42,
        longitude: 42
    },
    endLocationRegion: "Other Region",
    description: "Great description",
    category: DeliveryCategory.Other,
    isFragile: false,
})

const recipients = ref<string[]>([]);
const isInfoSection = ref(true);
const error = ref<string | undefined>();

const handleSwitchSection = (dto: AddDeliveryInfoDto) => {
    addDeliveryInfoDto.value = dto;
    isInfoSection.value = false;
};

const handleAddRecipients = (recipientUsernames: string[]) => {
    recipients.value = recipientUsernames;
    handleSubmit();
}

const handleSubmit = async () => {
    const token = useAuth().getAccessToken() ?? "";
    await useApi(`/deliveries`, {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${token}`,
        },
        body: {
            ...addDeliveryInfoDto.value,
            recipients: recipients.value,
        }
    }).catch((error) => {
        throw new Error("Error creating delivery: ", error);
    });
}

</script>

<template>
    <AddDeliveryInfo v-if="isInfoSection" :nextSectionHandler="handleSwitchSection"/>
    <AddRecipients v-else :error="error" :addRecipientsHandler="handleAddRecipients"/>
</template>