<template>
    <div class="login-container">
      <h2>Login</h2>
      <form @submit.prevent="login">
        <input v-model="email" type="email" placeholder="Email" required />
        <input v-model="password" type="password" placeholder="Password" required />
        <button type="submit">Login</button>
      </form>
      <p v-if="errorMessage">{{ errorMessage }}</p>
    </div>
  </template>
  
  <script>
  import api from "@/api";
  
  export default {
    data() {
      return {
        email: "",
        password: "",
        errorMessage: "",
      };
    },
    methods: {
        async login() {
  try {
    const response = await api.post("/login", {
      email: this.email,
      password: this.password,
    });

   

    // Verify if accessToken is present
    if (response.data && response.data.accessToken) {
      

      console.log('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAa');

      // Store tokens in localStorage
      localStorage.setItem("accessToken", response.data.accessToken);
      localStorage.setItem("refreshToken", response.data.refreshToken);

      // Set Authorization header
      api.defaults.headers.common["Authorization"] = `Bearer ${response.data.accessToken}`;

      // Redirect to timetracking page
      this.$router.push("/timetracking");
    } else {
      console.error("No access token found in response!");
      this.errorMessage = "Login failed: No token received.";
    }
  } catch (error) {
    console.error("Login Error:", error.response?.data || error.message);
    this.errorMessage = error.response?.data?.message || "Invalid credentials!";
  }
}


    },
  };
  </script>
  