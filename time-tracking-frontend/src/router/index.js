import { createRouter, createWebHistory } from "vue-router";

import LoginView from "@/views/LoginView.vue";
import TimeTracking from "@/views/TimeTracking.vue";

const routes = [
  {
    path: "/",
    component: MainLayout,
    children: [
      { path: "", name: "Login", component: LoginView },
      { path: "/dashboard", name: "Dashboard", component: TimeTracking },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
