<script setup lang="ts">
import { ref } from 'vue';
import type { ValidationRule } from 'vuetify/framework';

const router = useRouter();

const form = ref<HTMLFormElement>();

const username = ref('');
const usernameRules = ref<ValidationRule[]>([
	(input) => !!input || 'Username is required',
]);

const email = ref('');
const emailRules: ValidationRule[] = [
	(input) => {
		const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
		return emailPattern.test(input) || 'Invalid email format';
	},
];

const firstName = ref('');
const firstNameRules: ValidationRule[] = [
	(input) => !!input || 'First name is required',
];

const lastName = ref('');
const lastNameRules: ValidationRule[] = [
	(input) => !!input || 'Last name is required',
];

const phoneNumber = ref('');
const phoneNumberRules: ValidationRule[] = [
	(input) => {
		const phonePattern = /^\+?[1-9]\d{1,14}$/;
		return phonePattern.test(input) || 'Invalid phone number format';
	},
];

const password = ref('');
const passwordRules: ValidationRule[] = [
	(input: String) => {
		const minLength = 8;
		return (
			input.length >= minLength ||
			`Password must be at least ${minLength} characters long`
		);
	},
];

const confirmPassword = ref('');
const confirmPasswordRules: ValidationRule[] = [
	(input: String) => {
		return input === password.value || 'Passwords do not match';
	},
];

const isPasswordVisible = ref(false);

const error = ref<string | undefined>();

const register = async () => {
	const body = {
		username: username.value,
		password: password.value,
		firstName: firstName.value,
		lastName: lastName.value,
		email: email.value,
		phoneNumber: phoneNumber.value,
	};
	await useApi('/users', {
		method: 'POST',
		body: body,
	})
		.then(() => {
			error.value = undefined;
		})
		.catch(() => {
			error.value = "There was an error with the request";
		});
};

const submit = async () => {
	if (form.value?.validate()) {
		// Perform login action
		await register();
		if (error.value !== undefined) router.push('/login');
	} else {
		error.value = 'There was an error';
	}
};
</script>

<template>
	<v-container fluid class="d-flex align-center justify-center h-screen w-25">
		<v-card class="w-100">
			<v-card-title class="text-h4 text-center">Register</v-card-title>
			<v-card-text>
				<v-form
					class="d-flex flex-column align-center justify-center"
					ref="form"
					lazy-validation
				>
					<v-text-field
						v-model="username"
						:rules="usernameRules"
						variant="outlined"
						label="Username"
						class="w-100 pb-1"
						required
					></v-text-field>
					<v-text-field
						v-model="email"
						:rules="emailRules"
						variant="outlined"
						label="Email"
						class="w-100 pb-1"
						required
					></v-text-field>
					<v-text-field
						v-model="firstName"
						:rules="firstNameRules"
						variant="outlined"
						label="First name"
						class="w-100 pb-1"
						required
					></v-text-field>
					<v-text-field
						v-model="lastName"
						:rules="lastNameRules"
						variant="outlined"
						label="Last name"
						class="w-100 pb-1"
						required
					></v-text-field>
					<v-text-field
						v-model="phoneNumber"
						:rules="phoneNumberRules"
						variant="outlined"
						label="Phone number"
						class="w-100 pb-1"
						required
					></v-text-field>
					<v-text-field
						v-model="password"
						:rules="passwordRules"
						:append-inner-icon="
							isPasswordVisible ? 'mdi-eye' : 'mdi-eye-off'
						"
						:type="isPasswordVisible ? 'text' : 'password'"
						variant="outlined"
						label="Password"
						class="w-100 pb-1"
						@click:append-inner="
							isPasswordVisible = !isPasswordVisible
						"
						required
					></v-text-field>
					<v-text-field
						v-model="confirmPassword"
						:rules="confirmPasswordRules"
						:append-inner-icon="
							isPasswordVisible ? 'mdi-eye' : 'mdi-eye-off'
						"
						:type="isPasswordVisible ? 'text' : 'password'"
						variant="outlined"
						label="Confirm password"
						class="w-100 pb-1"
						@click:append-inner="
							isPasswordVisible = !isPasswordVisible
						"
						required
					></v-text-field>
					<p class="pb-2">
						Already have an account?
						<a class="text-blue" href="/login">Login here</a>
					</p>
					<p v-if="error" class="text-rose-400">{{ error }}</p>
					<v-btn
						@click="submit"
						variant="tonal"
						rounded="xl"
						color="primary"
						>Register</v-btn
					>
				</v-form>
			</v-card-text>
		</v-card>
	</v-container>
</template>

<style scoped></style>
