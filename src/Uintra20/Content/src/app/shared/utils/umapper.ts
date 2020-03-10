function umbracoMapper(obj) {
  const isArray = Array.isArray(obj);
  const iterable = isArray ? obj : Object.keys(isObject(obj) ? obj : {});

  iterable.forEach((value, i, arr) => {
    let cur = isArray ? value : obj[value];

    if (isObject(cur)) {
      if (cur.alias) {
        if (!isArray) {
          obj[value] = cur.value;
        } else {
          arr[i] = cur.value;
        }
        cur = cur.value;
      }

      umbracoMapper(isArray ? cur : obj[value]);
    }
  });
}

const isObject = obj => typeof obj === "object" && obj !== null;

export default umbracoMapper;
