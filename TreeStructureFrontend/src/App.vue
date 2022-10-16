<script setup>
import { ref, onMounted } from "vue";
import { VueFinalModal } from "vue-final-modal";
import NodeItem from "./components/NodeItem.vue";
import NodeAPI from "./NodeAPI.js";
import TreeFunctions from "./TreeFunctions.js";

const tree = ref({});
const loading = ref(true);
const rootKey = ref(0);

const inputid = ref("");
const inputname = ref("");

const showModal = ref(false);
const modalPayload = ref({
  isEdit: false,
  id: 0,
  parentId: 0,
  name: "",
});

const sortingModes = [
  "Name ascending",
  "Name descending",
  "Id ascending",
  "Id descending",
];

const sortIndex = ref(0);

const nodesToRefresh = ref([]);

const loadRoot = async () => {
  tree.value = TreeFunctions.mapSingle(await NodeAPI.getRoot());
  loading.value = false;
};

const loadAll = async () => {
  tree.value = TreeFunctions.mapAsNested(tree.value, await NodeAPI.getAll());
  rootKey.value += 1;
};

const toggleSort = () => {
  sortIndex.value = (sortIndex.value + 1) % sortingModes.length;
};

const testEvent = (payload) => {
  modalPayload.value = payload;

  inputid.value = payload.isEdit ? payload.parentId : payload.id;
  inputname.value = payload.isEdit ? payload.name : "";

  showModal.value = !showModal.value;
};

const errMessages = ref([]);
const modalSubmit = async () => {
  try {
    if (modalPayload.value.isEdit) {
      await NodeAPI.updateNode(modalPayload.value.id, {
        parentId: inputid.value,
        name: inputname.value,
      });
      nodesToRefresh.value = [inputid.value, modalPayload.value.parentId];
    } else {
      await NodeAPI.addNode({ parentId: inputid.value, name: inputname.value });
      nodesToRefresh.value = [inputid.value];
    }
    showModal.value = false;
  } catch (error) {
    errMessages.value = Object.values(error).flat();
  }
};

onMounted(() => {
  loadRoot();
});

</script>

<template>
  <header>
    <span>TREE STRUCTURE</span>
    <div class="buttondiv">
      <button @click="loadAll" class="header-button">Expand all</button>
      <button @click="toggleSort" class="header-button">
        {{ "Sort: " + sortingModes[sortIndex] }}
      </button>
    </div>
  </header>
  <main>
    <NodeItem
      @test-event="testEvent"
      v-if="!loading"
      :key="rootKey"
      :node="tree"
      :indent="0"
      :refresh="nodesToRefresh"
      :sortingModes="sortingModes[sortIndex]"
    />
    <div>
      <VueFinalModal
        v-model="showModal"
        classes="modal-container"
        content-class="modal-content"
        @closed="() => (errMessages = [])"
      >
        <span class="error-span" v-for="err in errMessages">{{ err }}</span>
        <div class="form-container">
          <div class="input-pair">
            <label class="modal-label" for="parentid">Parent ID</label>
            <input
              class="modal-input"
              type="number"
              id="parentid"
              name="parentid"
              :disabled="!modalPayload.isEdit || modalPayload.parentId === null"
              v-model="inputid"
            />
          </div>
          <div class="input-pair">
            <label class="modal-label" for="name">Name</label>
            <input
              class="modal-input"
              type="text"
              id="name"
              name="name"
              v-model="inputname"
            />
          </div>
          <button @click="modalSubmit" class="modal-button">SUBMIT</button>
        </div>
      </VueFinalModal>
    </div>
  </main>
</template>

<style scoped>
header {
  width: 100%;
  display: flex;
  flex-direction: column;
  place-items: center;
  line-height: 1.5;
  margin-bottom: 20px;
}

span {
  width: 100%;
  height: 70%;
  display: flex;
  place-content: center;
  font-size: 40px;
}

.buttondiv {
  width: 100%;
  height: 30%;
  display: flex;
  flex-direction: row;
  place-content: center;
}

.error-span {
  width: 80%;
  display: flex;
  flex-direction: row;
  justify-content: center;
  align-items: center;
  height: auto;
  font-size: 18px;
  margin-top: 5px;
  margin-bottom: 5px;
  color: red;
}

.form-container {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: space-around;
  align-items: center;
  font-size: 24px;
}

.input-pair {
  width: 100%;
}

.modal-label {
  width: 100%;
}
.modal-input {
  width: 100%;
  font-size: 24px;
}
.modal-button {
  width: 50%;
  height: 15%;
  font-size: 24px;
}

.header-button {
  width: 50%;
  height: 50%;
  margin: 20px;
  background-color: #333;
  color: #fff;
  font-size: 20px;
  transition: 300ms;
}
:deep(.modal-container) {
  height: 100vh;
  width: 100vw;
  display: flex;
  justify-content: center;
  align-items: center;
}
:deep(.modal-content) {
  height: 50%;
  width: 70%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  margin: 0 1rem;
  padding: 1rem;
  border: 1px solid #ddd;
  border-radius: 0.25rem;
  background: #111;
}
</style>
