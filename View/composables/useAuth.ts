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
        const {data, err} = await useFetch<LoginResponse>('/api/auth/login', {
            method: 'POST',
            body: {
                username,
                password
            }
        });

        // TODO: Improve error handling
        if (err.value) throw err.value;

        const {access, refresh} = data;

        setTokens(access, refresh);
    }

    const logout = () => {
        clearTokens()
    };

    const isLoggedIn = computed(() => getAccessToken() !== null);

    return {
        login,
        logout,
        getAccessToken,
        getRefreshToken,
        isLoggedIn,
    }
}

interface LoginResponse {
    accessToken: string;
    refreshToken: string;
}