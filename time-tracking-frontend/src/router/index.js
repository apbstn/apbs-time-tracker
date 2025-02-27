import { createRouter, createWebHistory } from 'vue-router';
import AppLayout from '../layout/AppLayout.vue'; // ✅ Import layout
import Home from '@/views/Home.vue';
import TimeTracking from '@/views/TimeTracking.vue';
import LoginView from '@/views/LoginView.vue';

const routes = [
  {
    path: '/',
    component: AppLayout, // ✅ Wrap main routes inside layout
    children: [
      { path: '/home', name: 'Home', component: Home, meta: { requiresAuth: true } },
      { path: '/timetracking', name: 'TimeTracking', component: TimeTracking, meta: { requiresAuth: true } }
    ]
  },
  { path: '/login', name: 'Login', component: LoginView } // ✅ Keep login separate
];

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior() {
    return { left: 0, top: 0 };
  }
});

// ✅ Global navigation guard
router.beforeEach((to, from, next) => {
  const isLoggedIn = !!localStorage.getItem('accessToken');

  if (to.meta.requiresAuth && !isLoggedIn) {
    next('/login'); // ✅ Redirect to login
  } else {
    next(); // ✅ Allow navigation
  }
});

export default router;
