<script setup lang="ts">

import AddDeliveryInfo from "~/components/delivery/AddDeliveryInfo.vue";
import {DeliveryCategory} from "~/components/delivery/types/DeliveryCategory";
import type {AddDeliveryInfoDto} from "~/components/delivery/types/AddDeliveryInfoDto";
import AddRecipients from "~/components/delivery/AddRecipients.vue";

const addDeliveryInfoDto = ref<AddDeliveryInfoDto>({
  packageName: "",
  startLocation: "",
  endLocation: "",
  description: "",
  category: DeliveryCategory.Other,
  isFragile: false,
})

const recipients = ref<string[]>([]);

const isInfoSection = ref(true);

const handleSwitchSection = (dto: AddDeliveryInfoDto) => {
  addDeliveryInfoDto.value = dto;
  isInfoSection.value = false;
};

const handleAddRecipients = (recipientUsernames: string[]) => {
  recipients.value = recipientUsernames;
  handleSubmit();
}

// TODO: Implement
const handleSubmit = () => {}

</script>

<template>
  <AddDeliveryInfo v-if="isInfoSection" :nextSectionHandler="handleSwitchSection" />
  <AddRecipients v-else :addRecipientsHandler="handleAddRecipients"/>
</template>