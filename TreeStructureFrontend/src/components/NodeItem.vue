<script setup>
import NodeAPI from "../NodeAPI.js";
import TreeFunctions from "../TreeFunctions.js";
import { ref, defineEmits, watch, computed } from "vue";

const props = defineProps({
  node: Object,
  indent: Number,
  refresh: Array,
  sortingModes: String,
});
const emit = defineEmits(["deleteNode", "testEvent"]);

const refNode = ref({ ...props.node });
const hasChildren = computed(() => {
  return refNode.value.children.length > 0 ? true : false;
});
const childrenShown = ref(hasChildren.value ? true : false);

const loadChildren = async () => {
  refNode.value = TreeFunctions.mapAsNested(
    refNode.value,
    await NodeAPI.getChildren(refNode.value.id)
  );
  sortChildren();
};

const sortChildren = () => {
  refNode.value.children = refNode.value.children.sort((a, b) => {
    switch (props.sortingModes) {
      case "Name ascending":
        return a.name.localeCompare(b.name);
      case "Name descending":
        return b.name.localeCompare(a.name);
      case "Id ascending":
        return a.id - b.id;
      case "Id descending":
        return b.id - a.id;
    }
  });
};

watch(
  () => props.refresh,
  (refresh) => {
    if (refresh.includes(refNode.value.id)) loadChildren();
  }
);

const notifyModal = (isEdit) => {
  const toSend = {
    isEdit: isEdit,
    id: refNode.value.id,
    parentId: refNode.value.parentId,
    name: refNode.value.name,
  };

  emit("testEvent", toSend);
};

const deleteNode = async () => {
  await NodeAPI.deleteNode(refNode.value.id);
  emit("deleteNode");
};
</script>

<template>
  <div :style="{ 'margin-left': indent + 'px' }" class="wrapper">
    <div class="node">
      <h2
        v-on:click="
          () => {
            loadChildren();
            childrenShown = !childrenShown;
          }
        "
      >
        {{
          (childrenShown ? "-" : "+") + "(ID:" + refNode.id + ")" + refNode.name
        }}
      </h2>

      <button @click="notifyModal(false)">Add to</button>
      <button v-if="refNode.parentId !== null" @click="notifyModal(true)">
        Edit/Move
      </button>
      <button v-if="refNode.parentId !== null" @click="deleteNode">
        Delete
      </button>
      <button @click="sortChildren" v-if="hasChildren">Sort children</button>
    </div>

    <template v-for="child in refNode.children" :key="child.id + child.name">
      <NodeItem
        v-show="childrenShown"
        @test-event="(arg) => emit('testEvent', arg)"
        @delete-node="loadChildren"
        :node="child"
        :indent="indent + 40"
        :refresh="props.refresh"
        :sortingModes="props.sortingModes"
      />
    </template>
  </div>
</template>

<style scoped>
.wrapper {
  width: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

.node {
  background-color: rgba(0, 0, 0, 0.2);
  width: 90%;
  height: 10vh;
  display: flex;
  flex-direction: row;
  justify-content: space-around;
  align-items: center;
  padding: 10px;
  margin-top: 10px;
  margin-bottom: 10px;
}

h2 {
  display: flex;
  place-content: center;
  width: 40%;
}

button {
  place-content: center;
  width: 12%;
  height: 80%;
  border-radius: 15%;
  font-size: 16px;
  background-color: #eee;
}
</style>
