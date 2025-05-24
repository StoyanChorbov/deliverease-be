import { baseUrl } from '~/composables/useApi';

export const useAuth = () => {
	const accessToken = useCookie('accessToken');
	const refreshToken = useCookie('refreshToken');
	const username = useCookie('username');

	const setTokens = (access: string, refresh: string) => {
		accessToken.value = access;
		refreshToken.value = refresh;
	};

	const setUsername = (value: string) => {
		username.value = value;
	};

	const getUsername = () => username.value;

	const getAccessToken = () => accessToken.value;
	const getRefreshToken = () => refreshToken.value;

	const clearCookies = () => {
		accessToken.value = null;
		refreshToken.value = null;
		username.value = null;
	};

	const login = async (username: string, password: string) => {
		const { data, error } = await useFetch<LoginResponse>(
			`${baseUrl}/users/login`,
			{
				method: 'POST',
				body: {
					username,
					password,
				},
			}
		);

		if (error.value) throw error.value;

		if (data.value == null) throw new Error('Unable to login');

		const { accessToken, refreshToken } = data.value;

		setTokens(accessToken, refreshToken);
		setUsername(username);
	};

	const logout = async () => {
        const token = getAccessToken() ?? '';
        await useApi('/users/logout', {
            method: 'POST',
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
		clearCookies();
	};

	const isAuthenticated = computed(
		() => accessToken.value !== null && accessToken.value !== undefined
	);

	return {
		login,
		logout,
		setTokens,
		setUsername,
		clearCookies,
		getUsername,
		getAccessToken,
		getRefreshToken,
		isAuthenticated: isAuthenticated,
	};
};

interface LoginResponse {
	accessToken: string;
	refreshToken: string;
}
