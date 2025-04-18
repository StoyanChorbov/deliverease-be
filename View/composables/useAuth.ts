export const useAuth = () => {
    const accessToken = 'accessToken';
    const refreshToken = 'refreshToken';

    const setTokens = (access: string, refresh: string) => {
        localStorage.setItem(accessToken, access);
        localStorage.setItem(refreshToken, refresh);
    }

    const getAccessToken = () => localStorage.getItem(accessToken);
    const getRefreshToken = () => localStorage.getItem(refreshToken);

    const clearTokens = () => {
        localStorage.removeItem(accessToken);
        localStorage.removeItem(refreshToken);
    };

    const login = async (username: string, password: string) => {
        const {data, error} = await useFetch<LoginResponse>('/api/auth/login', {
            method: 'POST',
            body: {
                username,
                password
            }
        });

        // TODO: Improve error handling
        if (error.value) throw error.value;

        if (data.value == null) throw new Error('Unable to login');

        const {accessToken, refreshToken} = data.value;

        setTokens(accessToken, refreshToken);

        console.log(getAccessToken());
        console.log(getRefreshToken());
    }

    const logout = () => {
        clearTokens()
    };

    const isAuthenticated = computed(() => getAccessToken() !== null);

    return {
        login,
        logout,
        getAccessToken,
        getRefreshToken,
        isAuthenticated,
    }
}

interface LoginResponse {
    accessToken: string;
    refreshToken: string;
}