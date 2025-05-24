<script setup lang="ts">
import {ref} from 'vue';
import type {ValidationRule} from "vuetify/framework";
import {useAuth} from "~/composables/useAuth";

const {login} = useAuth();
const router = useRouter();

const form = ref<HTMLFormElement>();

const username = ref('');
const usernameRules: ValidationRule[] = [
    input => {
        const minLength = 3;
        return input.length >= minLength || `Username must be at least ${minLength} characters long`;
    }
]

const password = ref('');
const isPasswordVisible = ref(false);
const passwordRules: ValidationRule[] = [
    (input: string) => {
        const minLength = 8;
        return input.length >= minLength || `Password must be at least ${minLength} characters long`;
    },
    (input: string) => {
        const hasAlphanumerical = /[a-zA-Z0-9]/.test(input);
        const hasSymbol = /[!@#$%^&*(),.?":{}|<>]/.test(input);
        return (hasAlphanumerical && hasSymbol) || 'Password must contain at least one uppercase letter, one lowercase letter, one number, and a special symbol';
    }
]

const error = ref("");

const submit = async () => {
    if (form.value?.validate()) {
        // Perform login action
        await login(username.value, password.value);
        router.push("/")
        error.value = "";
    } else {
        error.value = "There was an error when logging in";
    }
}

</script>

<template>
    <v-container fluid class="d-flex align-center justify-center h-screen w-25">
        <v-card class="w-100">
            <v-card-title class="text-h4 text-center">Login</v-card-title>
            <v-card-text>
                <v-form class="d-flex flex-column align-center justify-center" ref="form" lazy-validation>
                    <v-text-field
                        v-model="username"
                        :rules="usernameRules"
                        variant="outlined"
                        label="Username"
                        class="w-100"
                        required
                    ></v-text-field>
                    <v-text-field
                        v-model="password"
                        :rules="passwordRules"
                        :append-inner-icon="isPasswordVisible ? 'mdi-eye' : 'mdi-eye-off'"
                        :type="isPasswordVisible ? 'text' : 'password'"
                        variant="outlined"
                        label="Password"
                        class="w-100 pb-1"
                        @click:append-inner="isPasswordVisible = !isPasswordVisible"
                        required
                    ></v-text-field>
                    <p v-if="error.length > 0">{{ error }}</p>
                    <p>Don't have an account? <a class="text-blue" href="/register">Register here</a></p>
                    <v-btn @click="submit" variant="tonal" rounded="xl" color="primary">Login</v-btn>
                </v-form>
            </v-card-text>
        </v-card>
    </v-container>
</template>

<style scoped>

</style>