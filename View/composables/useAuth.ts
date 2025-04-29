import {baseUrl} from "~/composables/useApi";

export const useAuth = () => {
    const accessToken = useCookie('accessToken');
    const refreshToken = useCookie('refreshToken');

    const setTokens = (access: string, refresh: string) => {
        accessToken.value = access;
        refreshToken.value = refresh;
    }

    const getAccessToken = () => accessToken.value;
    const getRefreshToken = () => refreshToken.value;

    const clearTokens = () => {
        accessToken.value = null;
        refreshToken.value = null;
    };

    const login = async (username: string, password: string) => {
        const {data, error} = await useFetch<LoginResponse>(`${baseUrl}/users/login`, {
            method: 'POST',
            body: {
                username,
                password
            }
        });

        if (error.value) throw error.value;

        if (data.value == null) throw new Error('Unable to login');

        const {accessToken, refreshToken} = data.value;

        setTokens(accessToken, refreshToken);
    }

    const logout = () => {
        clearTokens()
    };

    const isAuthenticated = computed(() => accessToken.value !== null && accessToken.value !== undefined);

    return {
        login,
        logout,
        setTokens,
        clearTokens,
        getAccessToken,
        getRefreshToken,
        isAuthenticated,
    }
}

interface LoginResponse {
    accessToken: string;
    refreshToken: string;
}