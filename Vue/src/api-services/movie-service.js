import Axios from 'axios'

const RESOURCE_NAME = '/movies'

export default {
  getAll (page, limit, searchString) {
    return Axios.get(`${RESOURCE_NAME}?page=${page}&limit=${limit}&searchString=${searchString}`)
  },
  getAllSoon (page, limit, days) {
    return Axios.get(`${RESOURCE_NAME}/soon/${days}?page=${page}&limit=${limit}`)
  },
  get (id) {
    return Axios.get(`${RESOURCE_NAME}/${id}`)
  },
  create (data) {
    return Axios.post(RESOURCE_NAME, data)
  },
  update (id, data) {
    return Axios.put(`${RESOURCE_NAME}/${id}`, data)
  },
  delete (id) {
    return Axios.delete(`${RESOURCE_NAME}/${id}`)
  },
  getMoviesWithSeances (cinemaId, date) {
    return Axios.get(`${RESOURCE_NAME}/${cinemaId}/${date}`)
  },
  vote (data) {
    return Axios.post(`${RESOURCE_NAME}/vote`, data)
  },
  pref (data) {
    return Axios.post(`${RESOURCE_NAME}/pref`, data)
  },
  getUserMovieVote (id) {
    return Axios.get(`${RESOURCE_NAME}/vote/${id}`)
  },
  getMoviesPrediction (page, limit) {
    return Axios.get(`${RESOURCE_NAME}/predictions/?page=${page}&limit=${limit}`)
  }
}
