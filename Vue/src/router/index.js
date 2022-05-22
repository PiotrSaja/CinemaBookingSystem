import Vue from 'vue'
import Router from 'vue-router'
import Home from '@/components/Home'
import Movies from '@/components/Movies'
import MovieDetail from '@/components/MovieDetail'
import Profile from '@/components/Profile.vue'
import { authService } from '@/auth'
import NotFound from '@/components/error-pages/NotFound'
import NotAuth from '@/components/error-pages/NotAuth'
import Seances from '@/components/Seances'
import SeanceDetail from '@/components/SeanceDetail'
import BookingSeats from '@/components/BookingSeats'
import BookingUserInformation from '@/components/BookingUserInformation'
import BookingConfirmation from '@/components/BookingConfirmation'
import BookingDetail from '@/components/BookingDetail'
import Cinemas from '@/components/Cinemas'
import CinemaDetail from '@/components/CinemaDetail'

Vue.use(Router)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/movies',
    name: 'Movies',
    component: Movies
  },
  {
    path: '/movie/:id',
    name: 'MovieDetail',
    component: MovieDetail
  },
  {
    path: '/profile',
    name: 'Profile',
    component: Profile
  },
  {
    path: '/profile/booking/:id',
    name: 'BookingDetail',
    component: BookingDetail
  },
  {
    path: '/404',
    name: 'NotFound',
    component: NotFound,
    props: true
  },
  {
    path: '/401',
    name: 'NotAuth',
    component: NotAuth,
    props: true
  },
  {
    path: '/seances',
    name: 'Seances',
    component: Seances
  },
  {
    path: '/seance/:id',
    name: 'SeanceDetail',
    component: SeanceDetail
  },
  {
    path: '/booking-seats/:id',
    name: 'BookingSeats',
    component: BookingSeats
  },
  {
    path: '/booking/user-information',
    name: 'BookingUserInformation',
    component: BookingUserInformation,
    props: true
  },
  {
    path: '/booking/confirmation',
    name: 'BookingConfirmation',
    component: BookingConfirmation,
    props: true
  },
  {
    path: '/cinemas',
    name: 'Cinemas',
    component: Cinemas
  },
  {
    path: '/cinema/:id',
    name: 'CinemaDetail',
    component: CinemaDetail
  }
]

const router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

router.beforeEach((to, from, next) => {
  if (to.path === '/login') {
    authService.handleLoginRedirect()
      .then(() => next('/'))
      .catch(error => {
        console.log(error)
        next('/')
      })
  } else if (to.path === '/logout') {
    authService.handleLogoutRedirect()
      .then(() => next('/'))
      .catch(error => {
        console.log(error)
        next('/')
      })
  }

  next()
})

export default router
