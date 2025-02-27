<script setup>
import { ref, computed, onMounted } from "vue";
import AppMenuItem from "./AppMenuItem.vue";

const isLoggedIn = ref(false);

// Check login status when the component is mounted
onMounted(() => {
  isLoggedIn.value = !!localStorage.getItem("accessToken");
});

// Define menu items dynamically based on login status
const model = computed(() => [
  {
    label: "Dashboards",
    icon: "pi pi-home",
    items: [
      {
        label: "Home",
        icon: "pi pi-fw pi-chart-pie",
        to: "/Home",
        disabled: !isLoggedIn.value, // Disable if not logged in
      },
      {
        label: "Dashboard",
        icon: "pi pi-fw pi-chart-pie",
        to: "/timetracking",
        disabled: !isLoggedIn.value, // Disable if not logged in
      },
      isLoggedIn.value
        ? {
            label: "Logout",
            icon: "pi pi-fw pi-sign-out",
            command: () => logout(), // Call logout function
          }
        : {
            label: "Login",
            icon: "pi pi-fw pi-sign-in",
            to: "/login",
          },
    ],
  },
]);

// Logout function
const logout = () => {
  localStorage.removeItem("accessToken");
  isLoggedIn.value = false;
  window.location.href = "/login"; // Redirect to login page
};
</script>

<template>
  <ul class="layout-menu">
    <template v-for="(item, i) in model" :key="i">
      <AppMenuItem :item="item" root :index="i" />
      <li class="menu-separator"></li>
    </template>
  </ul>
</template>
