const baseUrl = "https://localhost:7110/api/node/";

export default {
  async getRoot() {
    const response = await fetch(baseUrl + "root", {
      method: "GET",
    });
    if (response.status != 200) {
      throw response.json();
    } else {
      return response.json();
    }
  },
  async getAll() {
    const response = await fetch(baseUrl + "all", {
      method: "GET",
    });
    const json = await response.json();
    if (response.status != 200) {
      return Promise.reject(json);
    } else {
      return Promise.resolve(json);
    }
  },
  async getChildren(id) {
    const response = await fetch(baseUrl + id, {
      method: "GET",
    });
    const json = await response.json();
    if (response.status != 200) {
      return Promise.reject(json);
    } else {
      return Promise.resolve(json);
    }
  },
  async deleteNode(id) {
    const response = await fetch(baseUrl + id, {
      method: "DELETE",
    });
    const json = await response.json();
    if (response.status != 204) {
      return Promise.reject(json);
    } else {
      return Promise.resolve();
    }
  },
  async addNode(data) {
    const response = await fetch(baseUrl, {
      headers: {
        Accept: "*/*",
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify(data),
    });
    const json = await response.json();
    if (response.status != 204) {
      return Promise.reject(json);
    } else {
      return Promise.resolve();
    }
  },
  async updateNode(id, data) {
    const response = await fetch(baseUrl + id, {
      headers: {
        Accept: "*/*",
        "Content-Type": "application/json",
      },
      method: "PUT",
      body: JSON.stringify(data),
    });
    const json = await response.json();
    if (response.status != 204) {
      return Promise.reject(json);
    } else {
      return Promise.resolve();
    }
  },
};
