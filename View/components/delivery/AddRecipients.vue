<script setup lang="ts">
const recipients = ref<string[]>([]);
const currentRecipient = ref<string>('');

interface AddRecipientsProps {
	error?: string;
	addRecipientsHandler: (recipients: string[]) => void;
	previousSectionHandler: (recipients: string[]) => void;
}

const props = defineProps<AddRecipientsProps>();

const addRecipient = () => {
	recipients.value.push(currentRecipient.value);
	currentRecipient.value = '';
};

const removeRecipient = (recipient: string) => {
	recipients.value.splice(currentRecipient.value.indexOf(recipient), 1);
};

const handlePrevious = () => {
	props.previousSectionHandler(recipients.value);
};
</script>

<template>
	<v-container>
		<v-text-field
			variant="outlined"
			v-model="currentRecipient"
			label="Add recipient"
		>
			<template v-slot:append-inner>
				<v-btn variant="plain" @click="addRecipient" icon="mdi-plus" />
			</template>
		</v-text-field>
		<v-text-field
			readonly
			v-for="recipient in recipients"
			variant="outlined"
			:key="recipient"
			:value="recipient"
			@click:append="removeRecipient(recipient)"
		>
			<template v-slot:append-inner>
				<v-btn
					variant="plain"
					@click="removeRecipient(recipient)"
					icon="mdi-delete"
				/>
			</template>
		</v-text-field>
		<p v-if="error">{{ error }}</p>
		<v-btn
			icon="mdi-arrow-left"
			variant="outlined"
			@click="handlePrevious"
		></v-btn>
		<v-btn
			@click="props.addRecipientsHandler(recipients)"
			variant="outlined"
			rounded="xl"
			color="primary"
		>
			Create delivery
		</v-btn>
	</v-container>
</template>
