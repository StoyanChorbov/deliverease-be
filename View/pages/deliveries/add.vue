<script setup lang="ts">

import AddDeliveryInfo from "~/components/delivery/AddDeliveryInfo.vue";
import type {AddDeliveryInfoDto} from "~/composables/delivery/types/AddDeliveryInfoDto";
import AddRecipients from "~/components/delivery/AddRecipients.vue";
import {useApi} from "#build/imports";

const addDeliveryInfoDto = ref<AddDeliveryInfoDto | {}>({})

const recipients = ref<string[]>([]);
const isInfoSection = ref(true);
const error = ref<string | undefined>();

const switchToRecipients = (dto: AddDeliveryInfoDto) => {
    addDeliveryInfoDto.value = dto;
    isInfoSection.value = false;
};

const switchToInfo = (recipientUsernames: string[]) => {
    recipients.value = recipientUsernames;
    isInfoSection.value = true;
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
    <AddDeliveryInfo v-if="isInfoSection" :nextSectionHandler="switchToRecipients"/>
    <AddRecipients v-else :error="error" :addRecipientsHandler="handleAddRecipients" :previousSectionHandler="switchToInfo"/>
</template>