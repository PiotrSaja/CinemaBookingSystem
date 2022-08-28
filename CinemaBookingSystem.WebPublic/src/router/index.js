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
    component: Movies,
    meta: {
      title: 'MyCinema - Movies'
    }
  },
  {
    path: '/movie/:id',
    name: 'MovieDetail',
    component: MovieDetail,
    meta: {
      title: 'MyCinema - Movies'
    }
  },
  {
    path: '/profile',
    name: 'Profile',
    component: Profile,
    meta: {
      title: 'MyCinema - Profile'
    }
  },
  {
    path: '/profile/booking/:id',
    name: 'BookingDetail',
    component: BookingDetail,
    meta: {
      title: 'MyCinema - Bookings'
    }
  },
  {
    path: '/404',
    name: 'NotFound',
    component: NotFound,
    props: true,
    meta: {
      title: 'MyCinema - Not Found'
    }
  },
  {
    path: '/401',
    name: 'NotAuth',
    component: NotAuth,
    props: true,
    meta: {
      title: 'MyCinema - Not Authorized'
    }
  },
  {
    path: '/seances',
    name: 'Seances',
    component: Seances,
    meta: {
      title: 'MyCinema - Seances'
    }
  },
  {
    path: '/seance/:id',
    name: 'SeanceDetail',
    component: SeanceDetail,
    meta: {
      title: 'MyCinema - Seances'
    }
  },
  {
    path: '/booking-seats/:id',
    name: 'BookingSeats',
    component: BookingSeats,
    meta: {
      title: 'MyCinema - Seats'
    }
  },
  {
    path: '/booking/user-information',
    name: 'BookingUserInformation',
    component: BookingUserInformation,
    props: true,
    meta: {
      title: 'MyCinema - Booking Information'
    }
  },
  {
    path: '/booking/confirmation',
    name: 'BookingConfirmation',
    component: BookingConfirmation,
    props: true,
    meta: {
      title: 'MyCinema - Booking Confirmation'
    }
  },
  {
    path: '/cinemas',
    name: 'Cinemas',
    component: Cinemas,
    meta: {
      title: 'MyCinema - Cinemas'
    }
  },
  {
    path: '/cinema/:id',
    name: 'CinemaDetail',
    component: CinemaDetail,
    meta: {
      title: 'MyCinema - Cinemas'
    }
  }
]

const router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

router.beforeEach((to, from, next) => {
  window.document.title = to.meta && to.meta.title ? to.meta.title : 'MyCinema - Home'

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
