import { createRouter, createWebHistory }  from 'vue-router'
import { authService } from '@/auth'
import Home from '@/views/Home'
import Cinemas from '@/views/Cinemas'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/cinemas',
    name: 'Cinemas',
    component: Cinemas
  }
]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes: routes
  })
  
router.beforeEach((to, from, next) => {
  if (to.path === '/login') {
    authService.handleLoginRedirect()
      .then(() =>{
      })
      .catch(error => {
        console.log(error)
      })
      next({
        path: '/'
      })
      return 
  } else if (to.path === '/logout') {
    authService.handleLogoutRedirect()
      .then(() => {
      })
      .catch(error => {
        console.log(error)
      })
      next({
        path: '/'
      })
      return 
  }

  next()
  return
})
export default router
