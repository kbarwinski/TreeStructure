export default {
  mapSingle(source) {
    let res = {
      id: source.id,
      name: source.name,
      parentId: source.parentId,
      children: [],
    };
    return res;
  },

  mapAsNested(target, source) {
    var children = source.filter((s) => s.parentId === target.id);

    var mappedChildren = children
      .map((c) => this.mapSingle(c))
      .map((c) => this.mapAsNested(c, source));

    let res = {
      ...target,
      children: mappedChildren,
    };

    return res;
  },
};
